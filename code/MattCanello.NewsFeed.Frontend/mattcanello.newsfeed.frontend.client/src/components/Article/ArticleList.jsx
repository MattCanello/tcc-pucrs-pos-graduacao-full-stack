import React from 'react';
import '../../style/ArticleList.css';
import Article from './Article';

function ArticleList({ articles }) {

    const articleList = (articles || []).map(article => <Article
        key={article.id}
        article={article}
    />);

    return (
        <section>
            {articleList}
        </section>
    );
}

export default ArticleList;