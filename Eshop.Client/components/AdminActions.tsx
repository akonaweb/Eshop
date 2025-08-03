import React, { useState } from "react";
import { IconButton, Menu, MenuItem, Tooltip } from "@mui/material";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";
import Link from "next/link";

import { useUserContext } from "./providers/AuthProvider";

const AdminActions = () => {
  const { session } = useUserContext();
  const isAdmin = session?.user?.name === "Administrator";

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  if (!isAdmin) return null;

  return (
    <>
      <Tooltip title="Administration">
        <IconButton
          onClick={handleOpen}
          size="large"
          edge="end"
          color="inherit"
        >
          <AdminPanelSettingsIcon />
        </IconButton>
      </Tooltip>
      <Menu
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        onClick={handleClose}
        transformOrigin={{ horizontal: "right", vertical: "top" }}
        anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
      >
        <MenuItem component={Link} href="/admin/orders">
          Orders
        </MenuItem>
        <MenuItem component={Link} href="/admin/categories">
          Categories
        </MenuItem>
        <MenuItem component={Link} href="/admin/products">
          Products
        </MenuItem>
      </Menu>
    </>
  );
};

export default AdminActions;
