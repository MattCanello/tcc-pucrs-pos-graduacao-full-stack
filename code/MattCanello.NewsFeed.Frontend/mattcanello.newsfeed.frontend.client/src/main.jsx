import React from 'react'
import ReactDOM from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import App from './App.jsx'
import { getQuerySearchParam } from './functions/Home.jsx'
import './index.css'
import ErrorPage from './components/Common/ErrorPage.jsx';
import ArticlePage from './components/Article/ArticlePage.jsx';
import HomePage from './components/Home/HomePage.jsx';
import ChannelPage from './components/Channel/ChannelPage.jsx';

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        errorElement: <ErrorPage />,
        loader: getQuerySearchParam,
        children: [
            {
                index: true,
                element: <HomePage />,
                loader: getQuerySearchParam
            },
            {
                path: "article/:feedId/:articleId",
                element: <ArticlePage />
            },
            {
                path: "channel/:channelId",
                element: <ChannelPage />
            }
        ]
    }
]);

const manifestLink = document.getElementById('webmanifestLink');
manifestLink.href = manifestLink.href
    .replace("/assets/", "/")
    .replace(/manifest-(?<x>\w+)\.webmanifest/is, "manifest.webmanifest");

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <RouterProvider router={router} />
    </React.StrictMode>,
)
