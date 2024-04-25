import { useRouteError } from "react-router-dom";
import Footer from './Footer';
import AppName from "./AppName";

export default function ErrorPage() {
    const error = useRouteError();
    console.log(error);

    function getMessage() {
        if (error.status == 404) {
            return "Página não encontrada";
        }

        return (
            <>
                <p>Aconteceu um erro inesperado.</p>
                <p>{error.statusText || error.message}</p>
            </>
        );
    }

    return (
        <main>
            <header>
                <AppName />
            </header>

            <section>
                <aside className="error">
                    {getMessage()}
                </aside>
            </section>

            <Footer />
        </main>
    );
}