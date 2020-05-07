import React from 'react';
import logo from './logo.svg';
import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  useParams
} from "react-router-dom";
import Layout from './layout';
import SecretConstructor from './components/construct-secret';
import SecretRetriever from './components/retrieve-secret';

function App() {
  return (
    <Router>
      <div>


        <Switch>
          <Route path="/:id" component={PleaseWork}>
          </Route>
          <Route path="/">
            <Layout children={Home()} />
          </Route>
        </Switch>
      </div>
    </Router>
  );
}

function Home() {
  return <SecretConstructor />;
}

const PleaseWork = ({ match }) => {
  return (<Layout children={<SecretRetriever secretId={match.params.id} />} />);
}



export default App;
