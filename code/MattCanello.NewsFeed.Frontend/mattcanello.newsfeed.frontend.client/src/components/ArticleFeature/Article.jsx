import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';
import ChannelNameAndPublishDate from './ChannelNameAndPublishDate';

function Article({ article }) {

    function renderThumbnailOrChannelAndPublishDate() {
        if (article.thumbnail === undefined) {
            return <ChannelNameAndPublishDate
                channelName={article.channel.channelName}
                publishDate={article.publishDate}
                />
        }

        return <Thumbnail
            channelName={article.channel.channelName}
            publishDate={article.publishDate}
            imageTitle={article.thumbnail.caption || article.thumbnail.credit || article.title}
            imageSrc={article.thumbnail.imageUrl}
            />
    }

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
            {renderThumbnailOrChannelAndPublishDate()}

            <ArticleTitle title={article.title} />

            {renderDetails()}

            <ArticleAuthor authors={getAuthorNames()} />
        </article>
    );
}

export default Article;