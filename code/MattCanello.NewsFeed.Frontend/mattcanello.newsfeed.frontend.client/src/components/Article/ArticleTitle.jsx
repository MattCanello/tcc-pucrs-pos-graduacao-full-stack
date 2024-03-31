import React from 'react';
import '../../style/ArticleTitle.css';
import { Link } from "react-router-dom";

function ArticleTitle({ title, feedId, articleId }) {
    return (
        <h2>
            <Link to={`/article/${feedId}/${articleId}`}>{title}</Link>
        </h2>
    );
}

export default ArticleTitle;