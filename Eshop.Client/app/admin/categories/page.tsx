import { Stack } from "@mui/material";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Link from "next/link";

import { getCategoriesSsr } from "@/api/categories";

export default async function AdminCategoriesPage() {
  const categories = await getCategoriesSsr();

  return (
    <>
      <h1>Management Categories</h1>

      <Stack gap={2} direction="column">
        <Link href={`/admin/categories/add`}>+ Add</Link>

        <div>
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Category ID</TableCell>
                  <TableCell>Name</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {categories.map((category) => (
                  <TableRow key={category.id}>
                    <TableCell>
                      <Link href={`/admin/categories/${category.id}`}>
                        {category.id}
                      </Link>
                    </TableCell>
                    <TableCell>{category.name}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
          <p>Total Categories: {categories.length}</p>
        </div>
      </Stack>
    </>
  );
}
