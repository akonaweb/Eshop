"use client";

import { ShoppingCart } from "@mui/icons-material";
import { Badge, Button } from "@mui/material";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

import { CartItem } from "@/api/orders";
import AdminActions from "./AdminActions";
import { useCartContext } from "./providers/CartProvider";
import { useUserContext } from "./providers/AuthProvider";

const ToolbarActions = () => {
  const router = useRouter();
  const { isLoading } = useUserContext();
  const { items: cartItems } = useCartContext();
  const [items, setItems] = useState<CartItem[]>([]);

  useEffect(() => {
    setItems(cartItems);
  }, [cartItems]);

  if (!items || isLoading) return <>Loading... </>;

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
