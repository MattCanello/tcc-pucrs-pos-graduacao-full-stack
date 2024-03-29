import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';

function Article({ title, channelName, publishDate, imageTitle, imageSrc, summary, description, authors }) {
  return (
      <article>
          <Thumbnail channelName={channelName} publishDate={publishDate} imageTitle={imageTitle} imageSrc={imageSrc} />

          <ArticleTitle title={title} />

          <ArticleDetails summary={summary} description={description} />

          <ArticleAuthor authors={authors} />
      </article>
  );
}

export default Article;