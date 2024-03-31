import React from 'react';
import '../../style/ReadMoreButton.css';

function ReadMoreButton({ url }) {
    function readMore() {
        return window.open(url, '_blank');
    }

    return (
        <button type="button" className="read-more" onClick={readMore}>
            Ler Mais
        </button>
    );
}

export default ReadMoreButton;