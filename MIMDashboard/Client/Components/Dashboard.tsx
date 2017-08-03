import * as React from 'react';
import { connect } from 'react-redux';
import { FetchData } from './PlayerStats/actions/player.action';


type stats = {
    newUsersDay: number,
    newUsersWeek: number,
    newUsersMonth: number,
    errors: any,
    hasErrored: boolean,
    isLoading: boolean

}

class Dashboard extends React.Component<stats> {
   
    
    componentDidMount() {
        this.props.FetchData('./client/Components/fake.json');
    }

    render() {

          if (this.state.props.isLoading) {
            return <p>Loading…</p>;
        }

        return (
            <div className="dashboard">


                <p>Dashboard, users: {this.state.props.newUsersDay}</p>

            </div>
        )
    }
}


const mapStateToProps = (state: stats) => {
    return {
        newUsersDay: state.newUsersDay,
        newUsersWeek: state.newUsersWeek,
        newUserMonth: state.newUsersMonth,
        errors: state.errors,
        hasErrored: state.hasErrored,
        isLoading: state.isLoading
    };
};

const mapDispatchToProps = (dispatch: any) => {
    return {
        FetchData: (url: string) => dispatch(FetchData(url))
    };
};


export default connect(mapStateToProps, mapDispatchToProps)(Dashboard);
