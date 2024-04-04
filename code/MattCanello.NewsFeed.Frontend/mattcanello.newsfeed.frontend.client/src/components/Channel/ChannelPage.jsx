import React from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicatorComponent from '../SignalR/NewArticlesIndicatorComponent';

function ChannelPage() {
    const articles = useLoaderData();

    return (
        <>
            <NewArticlesIndicatorComponent channelId={articles ? articles[0].channel.channelId : null} />

            <ArticleList articles={articles} />
        </>
    );
}

export default ChannelPage;