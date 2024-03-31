import React from 'react';
import { useEffect, useState } from 'react';
import '../../style/ChannelNavigation.css';

function ChannelNavigation() {
    const [channels, setChannels] = useState();

    useEffect(() => {
        populateChannels();
    }, []);

    function createListItems(channelList) {
        return channelList.map(channel => <li key={channel.channelId}><a href={channel.channelId}>{channel.name}</a></li>);
    }

    const channelData = channels === undefined
        ? createListItems([])
        : createListItems(channels);

    return (
        <nav>
            <ol>
                <li className="selected"><a href="#">Tudo</a></li>
                {channelData}
            </ol>
        </nav>
    );

    async function populateChannels() {
        const response = await fetch("channels");
        const data = await response.json();
        setChannels(data);
    }
}

export default ChannelNavigation;