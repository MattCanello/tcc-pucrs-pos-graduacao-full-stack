import React from 'react';
import '../../style/ArticleTitle.css';

function ArticleTitle({ title }) {
  return (
      <h2>
          <a href="#">
              {title}
          </a>
      </h2>
  );
}

export default ArticleTitle;