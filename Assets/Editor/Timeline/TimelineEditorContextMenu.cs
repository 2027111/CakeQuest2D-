using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor.Timeline;

[CustomEditor(typeof(TimelineAsset))]
public class TimelineEditorContextMenu : Editor
{
    // Declare a variable to store the track reference
    private static PlayableTrack dialogueLoopTrack;

    [MenuItem("CONTEXT/PlayableDirector/Create LoopPoints", false, 0)]
    private static void CreateLoopPoints(MenuCommand command)
    {
        PlayableDirector director = (PlayableDirector)command.context;
        if (director != null)
        {
            TimelineAsset timeline = director.playableAsset as TimelineAsset;
            if (timeline != null)
            {
                AddLoopPointsToSignals(timeline);
            }
        }
    }

    private static void AddLoopPointsToSignals(TimelineAsset timeline)
    {
        Debug.Log("Adding Loop Points");

        // Check if the track already exists
        dialogueLoopTrack = FindDialogueLoopTrack(timeline);

        if (dialogueLoopTrack == null)
        {
            // If the track doesn't exist, create it
            dialogueLoopTrack = timeline.CreateTrack<PlayableTrack>(null, "Dialogue Loop Track");
        }
        else
        {
            // Clear existing clips on the Dialogue Loop Track
            ClearExistingClips(dialogueLoopTrack);
        }

        // Iterate through the timeline's tracks
        foreach (var track in timeline.GetOutputTracks())
        {
            // Find signal tracks
            if (track is SignalTrack signalTrack)
            {
                Debug.Log("Signal Track: " + signalTrack.name);
                Debug.Log("Marker Count: " + signalTrack.GetMarkerCount());

                for (int i = 0; i < signalTrack.GetMarkerCount(); i++)
                {
                    // Check if the marker contains a SignalEmitter
                    if (signalTrack.GetMarker(i) is SignalEmitter signalEmitter)
                    {
                        if (signalEmitter.asset != null)
                        {
                            Debug.Log("Signal Name: " + signalEmitter.asset.name);
                            // Check if the SignalAsset's name matches "StartDialogueSequence"
                            if (signalEmitter.asset.name == "StartDialogueSequence")
                            {
                                // If the signal matches, create the loop playable for this signal at its time
                                CreateLoopPlayableForSignal(signalEmitter, timeline);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("SignalEmitter asset is null at index " + i);
                        }
                    }
                }
            }
        }
    }

    private static PlayableTrack FindDialogueLoopTrack(TimelineAsset timeline)
    {
        // Look for an existing track named "Dialogue Loop Track"
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is PlayableTrack playableTrack && playableTrack.name == "Dialogue Loop Track")
            {
                return playableTrack;
            }
        }
        return null; // Return null if the track is not found
    }

    private static void ClearExistingClips(PlayableTrack track)
    {
        // Iterate through existing clips and remove them
        foreach (var clip in track.GetClips())
        {
            track.DeleteClip(clip);
        }
        Debug.Log("Cleared existing clips from the 'Dialogue Loop Track'.");
    }

    private static void CreateLoopPlayableForSignal(SignalEmitter signalEmitter, TimelineAsset timeline)
    {
        // Add a DialogueLoopPlayableAsset to this track
        DialogueLoopPlayableAsset dialogueLoopPlayableAsset = ScriptableObject.CreateInstance<DialogueLoopPlayableAsset>();
        dialogueLoopPlayableAsset.pauseMethod = CutscenePauseMethod.Loop; // Set the desired pause method (Loop)

        // Check if the track exists, and if not, create it
        if (dialogueLoopTrack == null)
        {
            dialogueLoopTrack = timeline.CreateTrack<PlayableTrack>(null, "Dialogue Loop Track");
            Debug.Log("Created new Dialogue Loop Track.");
        }
        else
        {
            Debug.Log("Found existing Dialogue Loop Track.");
        }

        // Check if the track is valid before creating a clip
        if (dialogueLoopTrack != null)
        {
            // Now create the clip at the time of the signalEmitter (signalEmitter.time)
            TimelineClip clip = dialogueLoopTrack.CreateClip<DialogueLoopPlayableAsset>();

            if (clip != null)
            {
                // Add the DialogueLoopPlayableAsset to the timeline asset
                AssetDatabase.AddObjectToAsset(dialogueLoopPlayableAsset, timeline); // Add the asset to the timeline

                // Set properties for the clip
                clip.displayName = "DialogueLoopPlayableAsset";
                clip.duration = 0.2f; // Set clip duration
                clip.asset = dialogueLoopPlayableAsset;
                clip.start = signalEmitter.time; // Set the start time to match the signal emitter's time

                // Save the changes to the assets
                AssetDatabase.SaveAssets();

                // Mark the timeline as dirty to force the editor to update
                EditorUtility.SetDirty(timeline);

                // Refresh the timeline editor to reflect the changes immediately
                UnityEditor.Timeline.TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);

                Debug.Log($"Created Dialogue Loop Playable Asset for Signal at time: {signalEmitter.time}");
            }
            else
            {
                Debug.LogError("Failed to create clip for Dialogue Loop Track.");
            }
        }
        else
        {
            Debug.LogError("Dialogue Loop Track is null. Unable to create clip.");
        }
    }

}
