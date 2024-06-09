import "./Layout.css";
import Categories from "./Categories";
import Products from "./Products";
import { useState } from "react";

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
        <Products categoryId={activeCategoryId} />
      </div>
    </div>
  );
};

export default Layout;
