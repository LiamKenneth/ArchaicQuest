import * as React from 'react';
import * as ReactDom from 'react-dom';

type stats = {
    users: number
}

class Dashboard extends React.Component<stats> {
    constructor(props: stats) {
        super(props);

        this.state = {
            users: 10,
        }
    }


    render() {

        return (
            <div className="dashboard">
                <p>Dashboard, users: {this.state.users}</p>

            </div>
        )
    }
}
 

export default Dashboard;
