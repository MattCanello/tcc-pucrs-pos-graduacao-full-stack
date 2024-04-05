import React from 'react';
import '../../style/ArticleAuthor.css';

function ArticleAuthor({ authors }) {
    function renderAuthors() {
        return (authors)
            ? <strong>{authors}</strong>
            : null;
    }

    return renderAuthors();
}

export default ArticleAuthor;