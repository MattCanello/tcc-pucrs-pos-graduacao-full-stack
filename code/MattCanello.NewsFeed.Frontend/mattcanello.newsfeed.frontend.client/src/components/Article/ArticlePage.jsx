import React from 'react';
import { useState } from 'react';
import { useLoaderData } from "react-router-dom";
import Article from './Article';

function ArticlePage() {
    const [article] = useState(useLoaderData());

    const options = {
        expanded: true,
        displayShareButton: true,
        displayAuthorsRightBeforeTitle: true,
        displayReadMoreButton: true,
        useAbsoluteTime: true
    };

    return (
        <section>
            <Article article={article} options={options} />
        </section>
    );
}

export default ArticlePage;