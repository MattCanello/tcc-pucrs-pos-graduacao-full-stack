import { useEffect, useState } from 'react';
import './App.css';
import Header from './components/HomeFeature/Header';
import ArticleList from './components/ArticleFeature/ArticleList';

function App() {
    return (
        <main>
            <Header />

            <ArticleList />
        </main>
    );
}

export default App;