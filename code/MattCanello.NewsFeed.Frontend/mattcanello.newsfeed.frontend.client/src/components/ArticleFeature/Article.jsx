import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';

function Article({ article }) {
    function renderDetails() {
        if (article.details !== undefined) {
            return <ArticleDetails summary={article.details.summary} description={article.details.lines} />
        }

        return <ArticleDetails summary="" description={[]} />
    }

    function getAuthorNames() {
        if (article.authors === undefined) {
            return "";
        }

        return article.authors.map(author => author.name).join(", ");
    }

    return (
        <article>
            <Thumbnail
                channelName={article.channel.channelName}
                publishDate={article.publishDate}
                imageTitle={(article.thumbnail) ? (article.thumbnail.caption || article.thumbnail.credit || article.title) : article.title}
                imageSrc={(article.thumbnail) ? article.thumbnail.imageUrl : ''}
            />

            <ArticleTitle title={article.title} />

            {renderDetails()}

            <ArticleAuthor authors={getAuthorNames()} />
        </article>
    );
}

export default Article;