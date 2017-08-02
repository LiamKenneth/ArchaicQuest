import * as React from 'react';
import * as ReactDom from 'react-dom';
// import createStore  from 'redux';
// let store = createStore({}, ['Use Redux'])

import { GET_PLAYERS } from './PlayerStats/player.action';

type stats = {
    newUsersDay: number,
    newUsersWeek: number,
    newUserMonth: number,
    errors: any,
    loading: boolean

}

class Dashboard extends React.Component<stats> {
    constructor(props: stats) {
        super(props);

        this.state = {
            "newUsersDay": 0,
            "newUsersWeek": 0,
            "newUsersMonth": 0,
            "errors": null,
            "loading": true
        }
    }


    componentDidMount() {
        let component = this;
        fetch('./client/Components/fake.json')
            .then(function (response) {
                return response.json();
            })
            .then(function (json) {
            //   this.props.getPlayer()
               console.log(json)
               
            })
            .catch(function (exception) {
                console.log("Error fetching data: " + exception.message);
            });
    };


    render() {

        return (
            <div className="dashboard">
                <p>Dashboard, users: {this.state.newUsersDay}</p>

            </div>
        )
    }
}


export default Dashboard;
