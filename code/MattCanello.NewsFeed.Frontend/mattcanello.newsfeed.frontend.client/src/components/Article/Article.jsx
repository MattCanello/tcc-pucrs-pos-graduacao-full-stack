import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';

function Article({ article, options }) {
    function renderDetails() {
        if (article.details !== undefined) {
            return <ArticleDetails summary={article.details.summary} description={article.details.lines} expanded={options ? options.expanded : false} />
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
        <article className={options ? options.expanded ? "expanded" : "" : ""}>
            <Thumbnail
                channelName={article.channel.name}
                publishDate={article.publishDate}
                imageTitle={(article.thumbnail) ? (article.thumbnail.caption || article.thumbnail.credit || article.title) : article.title}
                imageSrc={(article.thumbnail) ? article.thumbnail.imageUrl : ''}
            />

            <ArticleTitle title={article.title} articleId={article.id} feedId={article.feed.feedId} />

            {renderDetails()}

            <ArticleAuthor authors={getAuthorNames()} />
        </article>
    );
}

export default Article;