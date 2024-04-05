import React from 'react';
import '../../style/ArticleList.css';
import Article from './Article';

function ArticleList({ articles, query }) {

    const articleList = (articles || []).map(article => <Article
        key={article.id}
        article={article}
    />);

    const emptyList = (articles || []).length == 0
        ? <aside className="empty">{query ? "A sua busca não produziu resultados" : "Parece que não há nenhum artigo por aqui"}</aside>
        : null;

    function renderOldFeed() {
        if (!articles || !articles.length || query) {
            return null;
        }

        const now = new Date();
        const publishDate = new Date(articles[0].publishDate);
        const thirtyDaysInMs = 30 * 24 * 60 * 60 * 1000;
        const timeDiffInMs = now.getTime() - publishDate.getTime();

        if (timeDiffInMs < thirtyDaysInMs) {
            return null;
        }

        return (
            <aside className="deprecated">
                Este canal não está mais atualizando o feed
            </aside>
        )
    }

    return (
        <section>
            {renderOldFeed()}

            {emptyList}

            {articleList}
        </section>
    );
}

export default ArticleList;