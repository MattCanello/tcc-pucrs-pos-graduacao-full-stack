import React from 'react';
import { useEffect, useState } from 'react';
import '../../style/ChannelNavigation.css';
import { NavLink } from "react-router-dom";
import { useLoaderData } from "react-router-dom";
import { getChannelList } from "../../functions/Channels";

function ChannelNavigation() {
    const { q } = useLoaderData();

    const [channels, setChannels] = useState();
    const [isExpanded, setIsExpanded] = useState(false);

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

    function toggleExpand() {
        setIsExpanded(!isExpanded);
    }

    function renderMenuButton() {
        if (channels === undefined || channels.length === undefined || channels.length < 10) {
            return null;
        }

        return (
            <button type="button" onClick={toggleExpand}>
                <span className="material-symbols-outlined">menu</span>
            </button>
        );
    }

    return (
        <nav className={isExpanded ? "expanded" : null}>
            {renderMenuButton()}
            <ol>
                {q ?
                    <li key="__search">
                        <NavLink to={"."} className={({ isActive, isPending }) => isActive ? "selected" : isPending ? "pending" : ""}>
                            {q ? "Resultado da busca" : "Tudo"}
                        </NavLink>
                    </li>
                    : null
                }

                <li key="__all">
                    <NavLink to={"/"} className={({ isActive, isPending }) => isActive && !q ? "selected" : isPending ? "pending" : ""}>
                        Tudo
                    </NavLink>
                </li>

                {channelData}
            </ol>
        </nav>
    );

    async function populateChannels() {
        const data = await getChannelList();
        setChannels(data);
    }
}

export default ChannelNavigation;