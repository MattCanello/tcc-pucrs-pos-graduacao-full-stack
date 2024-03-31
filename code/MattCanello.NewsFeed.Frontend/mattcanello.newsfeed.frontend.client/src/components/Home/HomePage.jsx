import React from 'react';
import { useState } from 'react';
import { useLoaderData } from "react-router-dom";
import ArticleList from '../Article/ArticleList';

function HomePage() {
    const [articles] = useState(useLoaderData());

    return (
        <ArticleList articles={articles} />
    );
}

export default HomePage;