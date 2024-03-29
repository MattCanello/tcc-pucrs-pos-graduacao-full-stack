import { useEffect, useState } from 'react';
import './App.css';
import Header from './components/HomeFeature/Header';
import ArticleList from './components/ArticleFeature/ArticleList';

function App() {
    const [articles, setArticles] = useState();

    useEffect(() => {
        getHomePageArticles();
    }, []);

    return (
        <main>
            <Header />

            <ArticleList articles={articles} />
        </main>
    );

    async function getHomePageArticles() {
        const response = await fetch("articles");
        const data = await response.json();
        setArticles(data);
    }
}

export default App;