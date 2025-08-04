import api, { getAccessToken } from "./core/api";
import urls from "./core/urls";
import useApiQuery from "./core/useApiQuery";

export type CartFullItem = {
  productId: number;
  productTitle: string;
  price: number;
  quantity: number;
  totalPrice: number;
};
export type Cart = {
  items: CartFullItem[];
  totalPrice: number;
};
export type CartItem = {
  productId: number;
  quantity: number;
};
export const getCart = async (items: CartItem[]): Promise<Cart> => {
  const response = await api(getAccessToken()).post(urls.order.cart, items);
  return response.data;
};
export function useCartQuery(items: CartItem[]) {
  return useApiQuery(["cart", JSON.stringify(items)], () => getCart(items), {
    enabled: items.length > 0,
  });
}

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

  const response = await api(getAccessToken()).post(
    urls.order.addOrder,
    payload
  );
  return response.data;
};

export type Order = {
  id: number;
  customer: string;
  address: string;
  createdAt: Date;
};
export const getOrders = async (
  ssrAccessToken: string | null
): Promise<Order[]> => {
  const response = await api(ssrAccessToken).get(urls.order.list);
  return response.data;
};
