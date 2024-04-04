import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import App from './App.jsx'
import { getHomePageArticles, searchArticles } from './functions/Home.jsx'
import './index.css'
import ErrorPage from './components/Common/ErrorPage.jsx';
import { getArticle } from './functions/Articles.jsx';
import ArticlePage from './components/Article/ArticlePage.jsx';
import HomePage from './components/Home/HomePage.jsx';
import ChannelPage from './components/Channel/ChannelPage.jsx';
import { getChannelArticles } from './functions/Channels.jsx';

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        errorElement: <ErrorPage />,
        loader: searchArticles,
        children: [
            {
                index: true,
                element: <HomePage />,
                loader: getHomePageArticles
            },
            {
                path: "article/:feedId/:articleId",
                element: <ArticlePage />,
                loader: getArticle
            },
            {
                path: "channel/:channelId",
                element: <ChannelPage />,
                loader: getChannelArticles
            }
        ]
    }
]);

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <RouterProvider router={router} />
    </React.StrictMode>,
)
