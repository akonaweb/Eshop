import Link from "next/link";
import { Stack } from "@mui/material";

import { getCategoriesSsr } from "@/api/categories";
import Table from "@/components/Table";

export default async function AdminCategoriesPage() {
  const categories = await getCategoriesSsr();

  const columns = [
    { key: "id", label: "Category ID", isLink: true },
    { key: "name", label: "Name" },
  ];

  return (
    <>
      <h1>Management Categories</h1>
      <Stack gap={2} direction="column">
        <Link href={`/admin/categories/add`}>+ Add</Link>
        <Table
          data={categories}
          columns={columns}
          baseHref="/admin/categories"
          entityName="Categories"
        />
      </Stack>
    </>
  );
}
