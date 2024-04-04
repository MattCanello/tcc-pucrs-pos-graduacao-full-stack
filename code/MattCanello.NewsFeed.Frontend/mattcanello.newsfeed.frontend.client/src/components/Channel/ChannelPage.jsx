import React from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicator from '../SignalR/NewArticlesIndicator';

function ChannelPage() {
    const articles = useLoaderData();

    return (
        <>
            <NewArticlesIndicator channelId={articles ? articles[0].channel.channelId : null} />

            <ArticleList articles={articles} />
        </>
    );
}

export default ChannelPage;