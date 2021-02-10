import { library } from "@fortawesome/fontawesome-svg-core";
import { fab } from "@fortawesome/free-brands-svg-icons";
import "App.scss";
import ContextRoute from "Context/ContextRoute";
import Layout from "Features/Home/Layout";
import StudentProfile from "Features/StudentProfile/StudentProfile";
import StudentTable from "Features/StudentTable/StudentTable";
import React from "react";
import { Switch } from "react-router-dom";
import { ROUTES } from "Utilities/Constants";
import { formatRoute } from "Utilities/HelperFunctions";
import "../node_modules/antd/dist/antd.css";

library.add(fab);

const App: React.FC = () => {
  return (
    <Layout>
      <Switch>
        <ContextRoute path={ROUTES.STUDENT_LIST} component={StudentTable} />
        <ContextRoute
          path={formatRoute(ROUTES.STUDENT_PAGE, ":studentId")}
          component={StudentProfile}
        />
        {/*You would add your error route here because /* is a catch all route */}
        <ContextRoute path="/*" component={StudentTable} />
      </Switch>
    </Layout>
  );
};

export default App;
