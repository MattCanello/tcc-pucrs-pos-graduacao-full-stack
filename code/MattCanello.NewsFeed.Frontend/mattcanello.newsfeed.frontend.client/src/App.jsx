import './App.css';
import Header from './components/Common/Header';
import { Outlet, useNavigation } from "react-router-dom";

function App() {
    const navigation = useNavigation();

    console.log('navigation', navigation);

    return (
        <main className={navigation.state === "loading" ? "loading" : ""}>
            <Header displayChannels={true} />

            <Outlet />
        </main>
    );
}

export default App;