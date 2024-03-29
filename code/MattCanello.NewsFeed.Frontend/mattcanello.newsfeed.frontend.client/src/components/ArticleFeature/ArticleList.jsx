import React from 'react';
import '../../style/ArticleList.css';
import Article from './Article';

function ArticleList({ articles }) {

    const articleList = (articles || []).map(article => <Article
        key={article.id}
        title={article.title}
        channelName={article.channel.channelName}
        publishDate={article.publishDate}
        imageTitle={article.thumbnail.caption || article.thumbnail.credit || article.title}
        imageSrc={article.thumbnail.imageUrl}
        summary={article.details.summary}
        description={article.details.lines}
        authors={article.authors.map(author => author.name).join(", ")}
    />);

    return (
        <section>
            {articleList}
        </section>
    );
}

export default ArticleList;