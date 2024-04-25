import React, { useEffect, useState } from 'react';
import Article from './Article';
import { useParams } from 'react-router';
import { getArticle } from '../../functions/Articles';

function ArticlePage() {
    const params = useParams();

    const [isLoading, setIsLoading] = useState(true);
    const [article, setArticle] = useState(null);

    useEffect(() => {
        setIsLoading(true);
        setArticle(null);

        getArticle({ params: params }).then(data => {
            setArticle(data);
            setIsLoading(false);
        });
    }, [params]);

    const options = {
        expanded: true,
        displayShareButton: true,
        displayAuthorsRightBeforeTitle: true,
        displayReadMoreButton: true,
        useAbsoluteTime: true
    };

    const isDocumentNotFound = (article && article.status && article.status === 404);

    return (
        <section>
            {(isDocumentNotFound || isLoading)
                ? <aside className="empty">{isDocumentNotFound ? "Artigo não encontrado" : "Carregando..."}</aside>
                : <Article article={article} options={options} />}
        </section>
    );
}

export default ArticlePage;