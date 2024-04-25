import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';
import ShareButton from './ShareButton';
import ReadMoreButton from './ReadMoreButton';
import ChannelNameAndPublishDate from './ChannelNameAndPublishDate';

function Article({ article, options }) {
    function renderDetails() {
        if (typeof article.details !== 'undefined' && article.details != null) {
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

    function renderShareButton() {
        if (options && options.displayShareButton) {
            return <ShareButton url={article.url} title={article.title} />
        }

        return null;
    }

    function renderReadMoreButton() {
        if (options && options.displayReadMoreButton) {
            return <ReadMoreButton url={article.url} />
        }

        return null;
    }

    return (
        <article className={options ? options.expanded ? "expanded" : "" : ""}>
            <Thumbnail
                imageTitle={(article.thumbnail) ? (article.thumbnail.caption || article.thumbnail.credit || article.title) : article.title}
                imageSrc={(article.thumbnail) ? article.thumbnail.imageUrl : ''}
                articleId={article.id}
                feedId={article.feed.feedId}
            />

            <ChannelNameAndPublishDate channelName={article.channel.name} publishDate={article.publishDate} useAbsoluteTime={options && options.useAbsoluteTime} />

            {renderShareButton()}

            <ArticleTitle title={article.title} articleId={article.id} feedId={article.feed.feedId} />

            {options && options.displayAuthorsRightBeforeTitle ? <ArticleAuthor authors={getAuthorNames()} /> : null}

            {renderDetails()}

            {!options || !options.displayAuthorsRightBeforeTitle ? <ArticleAuthor authors={getAuthorNames()} /> : null}

            {renderReadMoreButton()}
        </article>
    );
}

export default Article;