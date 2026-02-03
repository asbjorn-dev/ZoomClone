// 2 tracks in localTracks: audio and video
let localTracks = null;
// reference to the active twilio room
let activeRoom = null;

// token is for twilio access token for auth and containerId is HTML element id where video will render
async function connectToRoom(token, roomName, containerId) {
    // Force permission prompt + lock mic
    await navigator.mediaDevices.getUserMedia({
        audio: true,
        video: false
    });

    try {
        localTracks = await Twilio.Video.createLocalTracks({
            audio: true,
            video: true
        });

        // add local tracks to DOM immediately 
        const videoTrack = localTracks.find(track => track.kind === 'video');
        const audioTrack = localTracks.find(track => track.kind === 'audio');
        if (videoTrack) {
            addTrackToDOM(videoTrack, containerId);
        }
        if (audioTrack) {
            addTrackToDOM(audioTrack, containerId);
        }

    } catch (err) {
        console.warn("Falling back to audio-only:", err);

        localTracks = await Twilio.Video.createLocalTracks({
            audio: true,
            video: false
        });
    }


    // connects to twilio video room with token and tracks
    activeRoom = await Twilio.Video.connect(token, {
        name: roomName,
        tracks: localTracks
    });

    // attach video/audio for participants already in the room
    activeRoom.participants.forEach(participantConnected => {
        attachParticipantTracks(participantConnected, containerId);
    });

    // listen (events) for new participants connecting to the room
    activeRoom.on('participantConnected', participantConnected => {
        console.log(`Participant "${participantConnected.identity}" connected`);
        attachParticipantTracks(participantConnected, containerId);
    });

    // listen (events) for participants leaving the room
    activeRoom.on('participantDisconnected', participant => {
        console.log(`Participant "${participant.identity}" disconnected`);
        detachParticipantTracks(participant);
    });

    // listen for disconnection from the room
    activeRoom.on('disconnected', () => {
        console.log("Disconnected from the room");
        detachAllTracks(containerId);
    });
}

// func attach video/audio tracks from participants to the DOM
// participants is twilio object, containerId is html element id where video will render
function attachParticipantTracks(participant, containerId) {
    // attach already subscribed tracks
    participant.tracks.forEach(publication => {
        if (publication.isSubscribed) {
            addTrackToDOM(publication.track, containerId);
        }
    });

    // listen for new tracks being published by current participant
    participant.on('trackSubscribed', track => {
        addTrackToDOM(track, containerId);
    });

    // listen for tracks being unpublished by current participant and remove from DOM
    participant.on("trackUnsubscribed", track => {
        track.detach().forEach(element => element.remove());
    })
}

// func removes all tracks from a participant when they disconnect
function detachParticipantTracks(participant) {
    participant.tracks.forEach(publication => {
        if (publication.track) {
            // detach track from DOM
            publication.track.detach().forEach(element => element.remove());
        }
    });
}

// func add a video or audio track to the specified container element
function addTrackToDOM(track, containerId) {
    // finds the container element 
    const container = document.getElementById(containerId);

    if (!container) {
        console.error(`Container with id "${containerId}" not found`);
    }

    // create html element for the track (can be video or audio)
    const trackElement = track.attach();

    // add the track element to the container
    container.appendChild(trackElement);
    console.log(`Added track to container "${containerId}"`);
}

function detachAllTracks(containerId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    // Fjern alle video/audio elements fra DOM
    while (container.firstChild) {
        container.firstChild.remove();
    }

    // Stop local tracks (mic/cam)
    if (localTracks) {
        localTracks.forEach(track => {
            track.stop();
            track.detach?.().forEach(el => el.remove());
        });
        localTracks = null;
    }

    // Disconnect room cleanly
    if (activeRoom) {
        activeRoom.disconnect();
        activeRoom = null;
    }

    console.log("All tracks detached and room cleaned up");
}

// expose the connectToRoom function to be callable from Blazor
window.twilioVideo = {
    connectToRoom
};