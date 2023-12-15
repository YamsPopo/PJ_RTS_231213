using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineControler : MonoBehaviour
{
    public TimelineAsset timeline;
    TrackAsset[] track = new TrackAsset[3];
    PlayableDirector playable;
    void Start()
    {
        playable = GetComponent<PlayableDirector>();
        for (int i = 0; i < track.Length; i++)
        {
            track[i] = timeline.GetRootTrack(i);
            track[i].muted = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            track[0] = timeline.GetRootTrack(0);
            track[0].muted = false;
            track[1] = timeline.GetRootTrack(1);
            track[1].muted = true;
            track[2] = timeline.GetRootTrack(2);
            track[2].muted = true;
            playable.Play();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            track[0] = timeline.GetRootTrack(0);
            track[0].muted = true;
            track[1] = timeline.GetRootTrack(1);
            track[1].muted = false;
            track[2] = timeline.GetRootTrack(2);
            track[2].muted = true;
            playable.Play();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            track[0] = timeline.GetRootTrack(0);
            track[0].muted = true;
            track[1] = timeline.GetRootTrack(1);
            track[1].muted = true;
            track[2] = timeline.GetRootTrack(2);
            track[2].muted = false;
            playable.Play();
        }
    }
}