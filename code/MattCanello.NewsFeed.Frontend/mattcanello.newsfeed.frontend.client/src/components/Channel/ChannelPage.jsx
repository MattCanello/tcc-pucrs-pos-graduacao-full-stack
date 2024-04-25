import React, { useEffect, useState } from 'react';
import ArticleList from '../Article/ArticleList';
import NewArticlesIndicator from '../SignalR/NewArticlesIndicator';
import { getChannelArticles } from '../../functions/Channels.jsx';
import { useParams } from 'react-router';

function ChannelPage() {
    const params = useParams();

    const [isLoading, setIsLoading] = useState(true);
    const [articles, setArticles] = useState([]);

    useEffect(() => {
        setIsLoading(true);
        setArticles([]);

        getChannelArticles({ params: params }).then(data => {
            setArticles(data);
            setIsLoading(false);
        });
    }, [params]);


    return (
        <>
            <NewArticlesIndicator channelId={articles && articles.length ? articles[0].channel.channelId : null} />

            <ArticleList articles={articles} isLoading={isLoading} />
        </>
    );
}

export default ChannelPage;