import React from 'react';
import { useEffect, useState } from 'react';
import '../../style/ChannelNavigation.css';
import { Link, NavLink } from "react-router-dom";

function ChannelNavigation() {
    const [channels, setChannels] = useState();

    useEffect(() => {
        populateChannels();
    }, []);

    function createListItems(channelList) {
        return channelList.map(channel => createListItem(channel));
    }

    function createListItem(channel) {
        var className = "";

        return (
            <li key={channel.channelId} className={className}>
                <NavLink to={`/channel/${channel.channelId}`} className={({ isActive, isPending }) => isActive ? "selected" : isPending ? "pending" : ""}>
                    {channel.name}
                </NavLink>
            </li>
        );
    }

    const channelData = channels === undefined
        ? createListItems([])
        : createListItems(channels);

    return (
        <nav>
            <ol>
                <li className="selected"><Link to="/">Tudo</Link></li>
                {channelData}
            </ol>
        </nav>
    );

    async function populateChannels() {
        const response = await fetch("/channels");
        const data = await response.json();
        setChannels(data);
    }
}

export default ChannelNavigation;