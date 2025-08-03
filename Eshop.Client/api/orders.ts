import { CartItem } from "@/components/providers/CartProvider";
import api from "./api";
import urls from "./urls";
import { getClientAccessToken } from "./accessToken";

export type Cart = {
  items: CartFullItem[];
  totalPrice: number;
};

export type CartFullItem = {
  productId: number;
  productTitle: string;
  price: number;
  quantity: number;
  totalPrice: number;
};

export const getCart = async (items: CartItem[]): Promise<Cart> => {
  const resonse = await api(getClientAccessToken()).post(
    urls.order.cart,
    items
  );
  return resonse.data;
};

export type Customer = {
  name: string;
  address: string;
};
export const addOrder = async (
  items: CartItem[],
  customer: Customer
): Promise<Cart> => {
  const payload = {
    items,
    customer: customer.name,
    address: customer.address,
  };

  const resonse = await api(getClientAccessToken()).post(
    urls.order.addOrder,
    payload
  );
  return resonse.data;
};

export type Order = {
  id: number;
  customer: string;
  address: string;
  createdAt: Date;
};
export const getOrders = async (token: string | null): Promise<Order[]> => {
  const resonse = await api(token).get(urls.order.list);
  return resonse.data;
};
