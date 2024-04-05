import './App.css';
import Footer from './components/Common/Footer';
import Header from './components/Common/Header';
import { Outlet, useNavigation } from "react-router-dom";

function App() {
    const navigation = useNavigation();

    return (
        <main className={navigation.state === "loading" ? "loading" : ""}>
            <Header />

            <Outlet />

            <Footer />
        </main>
    );
}

export default App;