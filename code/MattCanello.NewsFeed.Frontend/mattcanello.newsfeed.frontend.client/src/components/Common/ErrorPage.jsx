import { useRouteError } from "react-router-dom";

export default function ErrorPage() {
    const error = useRouteError();
    console.log(error);

    return (
        <section>
            <h2>Ooops!</h2>

            <p>Aconteceu um erro inesperado.</p>

            <p>
                <em>{error.statusText || error.message}</em>
            </p>
        </section>
    );
}