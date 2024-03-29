import React from 'react';
import '../../style/Article.css';
import Thumbnail from './Thumbnail';
import ArticleDetails from './ArticleDetails';
import ArticleAuthor from './ArticleAuthor';
import ArticleTitle from './ArticleTitle';

function Article() {
  return (
      <article>
          <Thumbnail />

          <ArticleTitle />

          <ArticleDetails />

          <ArticleAuthor />
      </article>
  );
}

export default Article;