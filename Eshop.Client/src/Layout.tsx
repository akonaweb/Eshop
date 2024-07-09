import { useState } from "react";

import Categories from "./Categories";
import Products from "./Products";

import "./Layout.css";

const Layout = () => {
  const [activeCategoryId, setActiveCategoryId] = useState(1);

  return (
    <div className="layout-container">
      <div className="left-panel">
        <Categories
          activeCategoryId={activeCategoryId}
          onChange={setActiveCategoryId}
        />
      </div>

      <div className="right-panel">
        <Products activeCategoryId={activeCategoryId} />
      </div>
    </div>
  );
};

export default Layout;
