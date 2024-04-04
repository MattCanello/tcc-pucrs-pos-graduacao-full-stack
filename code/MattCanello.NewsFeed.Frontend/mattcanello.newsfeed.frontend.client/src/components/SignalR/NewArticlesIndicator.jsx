import React, { useState } from 'react';
import SignalRConnection from './SignalRConnection';
import { useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import '../../style/NewArticlesIndicator.css';

function NewArticlesIndicatorComponent({ channelId }) {
    const [newArticlesCount, setNewArticlesCount] = useState(0);

    const location = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        setNewArticlesCount(0);
    }, [location]);

    const events = SignalRConnection();

    function onNewArticleFound(article) {
        if (channelId !== undefined && article.channel.channelId !== channelId) {
            return 0;
        }

        setNewArticlesCount(newArticlesCount + 1);
        return 1;
    }

    useEffect(() => {
        events((article) => onNewArticleFound(article));
    });

    function onNewEntriesButtonClicked() {
        setNewArticlesCount(0);
        navigate(window.location.pathname, { replace: true });
        return true;
    }

    return (
        newArticlesCount == 0
            ? null
            : <button type="button" className="newEntries" onClick={onNewEntriesButtonClicked}>{newArticlesCount == 1 ? "1 novo artigo" : `${newArticlesCount} novos artigos`}</button>
    );
}

export default NewArticlesIndicatorComponent;