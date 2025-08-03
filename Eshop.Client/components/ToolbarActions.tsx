"use client";

import { ShoppingCart } from "@mui/icons-material";
import { Badge, Button } from "@mui/material";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

import { CartItem, useCartContext } from "./providers/CartProvider";
import AdminActions from "./AdminActions";

const ToolbarActions = () => {
  const router = useRouter();
  const { items: cartItems } = useCartContext();
  const [items, setItems] = useState<CartItem[]>([]);

  useEffect(() => {
    setItems(cartItems);
  }, [cartItems]);

  if (!items) return null;

  return (
    <>
      <Button
        onClick={() => {
          router.push("/cart");
        }}
      >
        <Badge badgeContent={items.length} color="primary">
          <ShoppingCart color="action" />
        </Badge>
      </Button>

      <AdminActions />
    </>
  );
};
export default ToolbarActions;
