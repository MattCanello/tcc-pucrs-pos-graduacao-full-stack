import React from 'react';
import '../../style/ArticleAuthor.css';

function ArticleAuthor({ authors }) {
    return (
        <strong>{authors}</strong>
    );
}

export default ArticleAuthor;