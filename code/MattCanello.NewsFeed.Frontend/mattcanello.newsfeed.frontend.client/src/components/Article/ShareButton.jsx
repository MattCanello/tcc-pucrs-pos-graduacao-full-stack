import React from 'react';
import '../../style/ShareButton.css';

function ShareButton({ url, title }) {
    async function openShare() {
        await navigator.share({
            url: url,
            text: title
        });

        return true;
    }

    if (navigator === undefined || navigator.share === undefined) {
        return (<></>);
    }

    return (
        <button type="button" className="share" onClick={openShare}>
            <span className="material-symbols-outlined">share</span>
        </button>
    );
}

export default ShareButton;