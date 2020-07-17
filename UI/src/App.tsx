import "../node_modules/antd/dist/antd.css";
import 'App.scss';
import React from 'react';
import { Switch } from 'react-router-dom';
import ContextRoute from 'Context/ContextRoute';
import Layout from 'Features/Home/Layout';
import { library } from '@fortawesome/fontawesome-svg-core'
import { fab } from '@fortawesome/free-brands-svg-icons'
import StudentTable from 'Features/StudentTable/StudentTable';
import { ROUTES } from 'Utilities/Constants';
import StudentProfile from "Features/StudentProfile/StudentProfile";
import { formatRoute } from "Utilities/HelperFunctions";
import { DashboardProvider } from "Context/DashboardContext";

library.add(fab);

const App: React.FC<{config: any}> = ({config}) => {
  return (
    <Layout>
      <DashboardProvider baseUrl={config.baseUrl}>
        <Switch>
          <ContextRoute path={ROUTES.STUDENT_LIST} component={StudentTable} />
          <ContextRoute path={formatRoute(ROUTES.STUDENT_PAGE, ":studentId")} component={StudentProfile} />
          {/*You would add your error route here because /* is a catch all route */}
          <ContextRoute path='/*' component={StudentTable} />
        </Switch>
      </DashboardProvider>
    </Layout>
  )
};

export default App;
