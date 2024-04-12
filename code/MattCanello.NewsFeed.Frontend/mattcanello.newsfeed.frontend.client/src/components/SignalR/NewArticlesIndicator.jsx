import React, { useState } from 'react';
import SignalRConnection from './SignalRConnection';
import { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import '../../style/NewArticlesIndicator.css';

function NewArticlesIndicatorComponent({ channelId }) {
    const [newArticlesCount, setNewArticlesCount] = useState(0);
    const [articlesQueue, setArticlesQueue] = useState([]);
        
    const location = useLocation();

    useEffect(() => {
        setNewArticlesCount(0);
        setArticlesQueue([]);
    }, [location]);

    const events = SignalRConnection();

    useEffect(() => {
        function onNewArticleFound(article) {
            if (channelId !== undefined && article.channel.channelId !== channelId) {
                return 0;
            }

            const newQueue = [...articlesQueue];
            newQueue.push(article);
            setArticlesQueue(newQueue);

            setNewArticlesCount(newArticlesCount + 1);
            return 1;
        }

        events((article) => onNewArticleFound(article));
    }, [newArticlesCount, articlesQueue, events, channelId]);

    function onNewEntriesButtonClicked() {
        var event = new CustomEvent("dequeueArticles", { detail: { articles: articlesQueue } });
        document.dispatchEvent(event);

        setArticlesQueue([]);
        setNewArticlesCount(0);
        return true;
    }

    return (
        newArticlesCount == 0
            ? null
            : <button type="button" className="newEntries" onClick={onNewEntriesButtonClicked}>{newArticlesCount == 1 ? "1 novo artigo" : `${newArticlesCount} novos artigos`}</button>
    );
}

export default NewArticlesIndicatorComponent;