import React from 'react';
import '../../style/ArticleList.css';
import Article from './Article';

function ArticleList({ articles, query, isLoading }) {

    const articleList = (articles || []).map(article => <Article
        key={article.id}
        article={article}
    />);

    function getEmptyMessage() {
        if (isLoading && query) {
            return "Buscando...";
        }

        if (isLoading) {
            return "Carregando...";
        }

        if (query) {
            return "A sua busca não produziu resultados";
        }

        return "Parece que não há nenhum artigo por aqui";
    }

    const emptyList = (articles || []).length == 0
        ? <aside className="empty">{getEmptyMessage()}</aside>
        : null;

    function renderOldFeed() {
        if (!articles || !articles.length || query) {
            return null;
        }

        const now = new Date();
        const publishDate = new Date(articles[0].publishDate);
        const twentyDaysInMs = 20 * 24 * 60 * 60 * 1000;
        const timeDiffInMs = now.getTime() - publishDate.getTime();

        if (timeDiffInMs < twentyDaysInMs) {
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