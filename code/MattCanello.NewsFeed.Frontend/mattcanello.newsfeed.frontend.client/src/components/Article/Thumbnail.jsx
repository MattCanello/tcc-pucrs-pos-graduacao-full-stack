import React from 'react';
import '../../style/Thumbnail.css';
import { useNavigate } from "react-router-dom";

function Thumbnail({ imageTitle, imageSrc, articleId, feedId }) {

    const navigate = useNavigate();

    const imgContent = (imageSrc)
        ? <img loading="lazy" title={imageTitle} src={imageSrc} />
        : null;

    const figCaptionContent = (imageSrc)
        ? <figcaption>{imageTitle}</figcaption>
        : null;

    function isArticlePage() {
        return window.location.pathname === `/article/${feedId}/${articleId}`;
    }

    function navigateToArticle() {
        if (isArticlePage()) {
            return false;
        }

        navigate(`/article/${feedId}/${articleId}`);
        return true;
    }

    return (
        <figure onClick={navigateToArticle} className={isArticlePage() ? "" : "pointer"}>
            {imgContent}
            {figCaptionContent}
        </figure>
    );
}

export default Thumbnail;